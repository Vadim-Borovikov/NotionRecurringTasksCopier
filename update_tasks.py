from datetime import *; from dateutil.relativedelta import *
import calendar
from math import floor
from notion.client import NotionClient
from notion.collection import NotionDate, CollectionRowBlock

def update_tasks(token_v2, source_page_url, target_page_url):
    client = NotionClient(token_v2)
    source_block = client.get_block(source_page_url)
    source = source_block.collection
    target_block = client.get_block(target_page_url)
    target = target_block.collection
    earliest_date = date.today() + relativedelta(months=-1, day=1)
    latest_date = earliest_date + relativedelta(months=+2, weeks=+1)
    recurring_rows = [r for r in source.get_rows() if r.test]
    for num, row in enumerate(recurring_rows):
        print("Recurring {0} ({1}/{2})...".format(row.title, num + 1, len(recurring_rows)))
        update_next(row)
        ensure_recurring(row, latest_date, target)

#------------------------------------------------------------

def update_next(row):
    row.predydushchie = [r for r in row.predydushchie if r.alive]
    row.sleduiushchie = [r for r in row.sleduiushchie if r.alive]
    previous_rows = row.predydushchie
    next_rows = row.sleduiushchie
    for next in row.sleduiushchie:
        if get_date(next.data.start) < date.today():
            previous_rows.append(next)
            next_rows.remove(next)
    client = row.collection._client
    with client.as_atomic_transaction():
        row.predydushchie = previous_rows
        row.sleduiushchie = next_rows

def ensure_recurring(source_row, latest_date, target):
    next_dates = []
    for mode in source_row.povtoriaetsia:
        for next_date in get_next_dates(mode, get_previous_date(source_row), latest_date):
            if next_date >= date.today() and not has_recurring(source_row.sleduiushchie, next_date):
                next_dates.append(next_date)
    next_rows = source_row.sleduiushchie
    for num, next_date in enumerate(next_dates):
        print("    {0}/{1}...".format(num + 1, len(next_dates)))
        properties = prepare_properties(source_row, next_date)
        new_row = create_row(target, properties)
        next_rows.append(new_row)
    source_row.sleduiushchie = next_rows

def has_recurring(next_rows, next_date):
    return any(r for r in next_rows if get_date(r.data.start) == next_date)

#------------------------------------------------------------

def prepare_properties(row, next_date):
    properties = row.get_all_properties()
    properties['status'] = 'Не приступал'
    properties['data'] = get_new_date(row.data, next_date)
    del properties['povtoriaetsia']
    del properties['predydushchie']
    del properties['sleduiushchie']
    return properties

def create_row(collection, properties):
    client = collection._client
    row_id = client.create_record("block", collection, type="page")
    new_row = CollectionRowBlock(client, row_id)
    with client.as_atomic_transaction():
        for p in properties:
            setattr(new_row, p, properties[p])
    return new_row

#------------------------------------------------------------

def get_previous_date(row):
    date = row.data.start
    if row.predydushchie:
        date = max([r.data.start for r in row.predydushchie])
    return get_date(date)

def get_new_date(notion_date, next_date):
    days_diff = next_date - get_date(notion_date.start)
    new_start = notion_date.start + days_diff
    new_end = None
    if not notion_date.end is None:
        new_end = notion_date.end + days_diff
    return NotionDate(new_start, new_end, notion_date.timezone)    

def get_date(day):
    if isinstance(day, datetime):
        return day.date()
    else:
        return day

def get_next_dates(mode, other_day_seed, end):
    start = date.today()
    if mode == 'Понедельники':
        return get_weekly_days(0, start, end)
    elif mode == 'Вторники':
        return get_weekly_days(1, start, end)
    elif mode == 'Среды':
        return get_weekly_days(2, start, end)
    elif mode == 'Четверги':
        return get_weekly_days(3, start, end)
    elif mode == 'Пятницы':
        return get_weekly_days(4, start, end)
    elif mode == 'Субботы':
        return get_weekly_days(5, start, end)
    elif mode == 'Воскресенья':
        return get_weekly_days(6, start, end)
    elif mode == 'Каждый второй понедельник' or mode == 'Каждое второе воскресенье':
        return get_weekly_other_days(start, end, other_day_seed)
    elif mode == 'Каждое 1е':
        return get_next_days(1, start, end)
    elif mode == 'Каждое 20е':
        return get_next_days(20, start, end)
    raise ValueError('Undefined mode: {}'.format(mode))

def get_weekly_days(weekday, start, end):
    start = start + relativedelta(weekday=weekday)
    return get_days(start, end, 7)

def get_weekly_other_days(start, end, seed):
    steps = 0;
    if seed < start:
        steps = floor((start - seed).days / 14) + 1
    start = seed + timedelta(days=(14 * steps))
    return get_days(start, end, 14)

def get_days(start, end, step_days):
    next = start
    while next < end:
        yield next
        next += timedelta(days=step_days)

def get_next_weekday_since(weekday, last):
    days_ahead = weekday - last.weekday()
    if days_ahead <= 0:
        days_ahead += 7
    return last + timedelta(days_ahead)

def get_second_next_weekday_since(weekday, last):
    next = get_next_weekday_since(weekday, last)
    return next + timedelta(7)

def get_next_days(day, start, end):
    next = start + relativedelta(months=0, day=day)
    while next < end:
        yield next
        next += relativedelta(months=+1)