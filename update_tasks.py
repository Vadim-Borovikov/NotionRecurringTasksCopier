from datetime import date, datetime, timedelta
from dateutil.relativedelta import relativedelta, SU
from math import floor
from notion.client import NotionClient
from notion.collection import NotionDate, CollectionRowBlock

def update_tasks(token_v2, source_page_url, target_page_url, test=False):
    client = NotionClient(token_v2)
    source_block = client.get_block(source_page_url)
    source = source_block.collection
    target_block = client.get_block(target_page_url)
    target = target_block.collection
    earliest_date = get_earliest_date(date.today())
    latest_date = get_latest_date(date.today())
    recurring_rows = source.get_rows()
    if test:
        recurring_rows = [r for r in source.get_rows() if r.test]
    for num, row in enumerate(recurring_rows):
        print("Recurring {0} ({1}/{2})...".format(row.title, num + 1, len(recurring_rows)))
        ensure_recurring(row, latest_date, target)

#------------------------------------------------------------

def get_earliest_date(date):
    return date + relativedelta(months=-1, day=1)

def get_latest_date(date):
    latest_date = date + relativedelta(months=+1, day=1, weekday=SU(1))
    if latest_date.day < 7:
        latest_date += relativedelta(weeks=+1)
    return latest_date    

def ensure_recurring(source_row, latest_date, target):
    next_dates = []
    day_after_latest = latest_date + relativedelta(days=+1)
    start = get_date(source_row.data.start)
    for mode in source_row.povtoriaetsia:
        previous_date = get_date(source_row.data.start)
        for next_date in get_next_dates(mode, previous_date, day_after_latest):
            if next_date >= date.today() and not has_recurring(source_row, target.get_rows(), next_date):
                next_dates.append(next_date)
    for num, next_date in enumerate(next_dates):
        print("    Adding {2:%d.%m.%Y} ({0}/{1})...".format(num + 1, len(next_dates), next_date))
        properties = prepare_properties(source_row, next_date)
        create_row(target, properties)

def has_recurring(source_row, target_rows, date):
    title = source_row.title
    return any(r for r in target_rows if r.povtoriaetsia and r.title == title and get_date(r.data.start) == date)

#------------------------------------------------------------

def prepare_properties(row, next_date):
    properties = row.get_all_properties()
    properties['status'] = 'Не приступал'
    properties['data'] = get_new_date(row.data, next_date)
    properties['povtoriaetsia'] = True
    del properties['kommentarii']
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
        return get_weekly_other_days(start, end)
    elif mode == 'Каждое 1е':
        return get_next_days(1, start, end)
    elif mode == 'Каждое 20е':
        return get_next_days(20, start, end)
    elif mode == 'Каждые полгода':
        return get_next_months(6, start, end)
    raise ValueError('Undefined mode: {}'.format(mode))

def get_weekly_days(weekday, start, end):
    start = start + relativedelta(weekday=weekday)
    return get_days(start, end, 7)

def get_weekly_other_days(start, end):
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
        
def get_next_months(step_months, start, end):
    next = start
    while next < end:
        yield next
        next += relativedelta(months=+step_months)