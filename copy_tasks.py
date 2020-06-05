from datetime import *; from dateutil.relativedelta import *
import calendar
from notion.client import NotionClient
from notion.collection import NotionDate, CollectionRowBlock

def copy_tasks(token_v2, page_url):
    client = NotionClient(token_v2)
    block = client.get_block(page_url)
    tasks = block.collection
    earliest_date = date.today() + relativedelta(months=-1, day=1)
    latest_date = earliest_date + relativedelta(months=+3)
    for row in tasks.get_rows():
        if need_recurring(row, earliest_date, latest_date):
            ensure_recurring(row, latest_date)

#------------------------------------------------------------

def need_recurring(row, earliest_date, latest_date):
    if row.povtoriaetsia == []:
        return False
    return earliest_date <= get_date(row.data.start) < latest_date

def ensure_recurring(row, latest_date):
    for mode in row.povtoriaetsia:
        date = get_date(row.data.start)
        for next_date in get_next_dates(mode, date, latest_date):
            if next_date >= date.today() and not has_recurring(row, next_date):
                create_recurring(row, next_date)

def has_recurring(row, next_date):
    for r in row.collection.get_rows():
        if r.title != row.title:
            continue
        if next_date == get_date(r.data.start):
            return True
    return False

def create_recurring(row, next_date):
    properties = prepare_properties(row, next_date)
    return create_row(row.collection, properties)

#------------------------------------------------------------

def prepare_properties(row, next_date):
    properties = row.get_all_properties();
    properties['status'] = 'Не приступал'
    days_diff = next_date - get_date(row.data.start)
    new_start = row.data.start + days_diff
    new_end = None
    if not row.data.end is None:
        new_end = row.data.end + days_diff
    properties['data'] = NotionDate(new_start, new_end, row.data.timezone)
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
    print(start, end)
    next = start + relativedelta(months=0, day=day)
    while next < end:
        yield next
        next += relativedelta(months=+1)