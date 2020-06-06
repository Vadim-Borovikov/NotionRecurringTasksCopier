from datetime import *; from dateutil.relativedelta import *
import calendar
from notion.client import NotionClient
from notion.collection import NotionDate, CollectionRowBlock

def copy_tasks(token_v2, page_url):
    client = NotionClient(token_v2)
    block = client.get_block(page_url)
    collection = block.collection
    earliest_date = date.today() + relativedelta(months=-1, day=1)
    latest_date = earliest_date + relativedelta(months=+3)
    recurring_rows = [row for row in collection.get_rows() if need_recurring(row, earliest_date, latest_date)]
    for num, row in enumerate(recurring_rows):
        print("Recurring {0} ({1}/{2})...".format(row.title, num + 1, len(recurring_rows)))
        ensure_recurring(row, latest_date)

#------------------------------------------------------------

def need_recurring(row, earliest_date, latest_date):
    if row.povtoriaetsia == []:
        return False
    return earliest_date <= get_date(row.data.start) < latest_date

def ensure_recurring(row, latest_date):
    date = get_date(row.data.start)
    next_dates = []
    for mode in row.povtoriaetsia:
        for next_date in get_next_dates(mode, date, latest_date):
            if next_date >= date.today() and not has_recurring(row, next_date):
                next_dates.append(next_date)
    for num, next_date in enumerate(next_dates):
        print("    {0}/{1}...".format(num + 1, len(next_dates)))
        properties = prepare_properties(row, next_date)
        create_row(row.collection, properties)

def has_recurring(row, next_date):
    for r in row.collection.get_rows():
        if r.title != row.title:
            continue
        if next_date == get_date(r.data.start):
            return True
    return False

#------------------------------------------------------------

def prepare_properties(row, next_date):
    properties = row.get_all_properties();
    properties['status'] = 'Не приступал'
    properties['data'] = get_new_date(row.data, next_date)
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

def get_next_dates(mode, start, end):
    start = get_date(start)
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
    elif mode == 'Каждый второй понедельник':
        return get_weekly_other_days(0, start, end)
    elif mode == 'Каждое второе воскресенье':
        return get_weekly_other_days(6, start, end)
    elif mode == 'Каждое 1е':
        return get_next_days(1, start, end)
    elif mode == 'Каждое 20е':
        return get_next_days(20, start, end)
    raise ValueError('Undefined mode: {}'.format(mode))

def get_weekly_days(weekday, start, end):
    if start.weekday() != weekday:
        return []
    return get_days(start, end, 7)

def get_weekly_other_days(weekday, start, end):
    if start.weekday() != weekday:
        return []
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