from datetime import date, datetime, timedelta
from dateutil import relativedelta
from notion.client import NotionClient
from notion.collection import NotionDate, CollectionRowBlock

def copy_tasks(token_v2, page_url):
    client = NotionClient(token_v2)
    block = client.get_block(page_url)
    tasks = block.collection
    for row in tasks.get_rows():
        if need_recurring(row):
            ensure_recurring(row)

#------------------------------------------------------------

def need_recurring(row):
    if row.povtoriaetsia == []:
        return False
    days_passed = get_days_passed(row.data.start)
    return 0 < days_passed < 14

def ensure_recurring(row):
    next_dates = [get_next_date(mode, row.data) for mode in row.povtoriaetsia]
    for next_date in next_dates:
        if not has_recurring(row, next_date):
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

def get_days_passed(day):
    time_passed = date.today() - get_date(day)
    return time_passed.days

def get_next_date(mode, notion_date):
    if mode == 'Понедельники':
        date = get_next_weekday(0);
    elif mode == 'Вторники':
        date = get_next_weekday(1);
    elif mode == 'Среды':
        date = get_next_weekday(2);
    elif mode == 'Четверги':
        date = get_next_weekday(3);
    elif mode == 'Пятницы':
        date = get_next_weekday(4);
    elif mode == 'Субботы':
        date = get_next_weekday(5);
    elif mode == 'Воскресенья':
        date = get_next_weekday(6);
    elif mode == 'Каждое второе воскресенье':
        date = get_second_next_weekday_since(6, notion_date.start);
    elif mode == 'Каждое 1е':
        date = get_next_day(1);
    elif mode == 'Каждое 20е':
        date = get_next_day(20);
    else:
        raise ValueError('Undefined mode: {}'.format(mode))
    return date

def get_date(day):
    if isinstance(day, datetime):
        return day.date()
    else:
        return day

def get_next_weekday_since(weekday, last):
    days_ahead = weekday - last.weekday()
    if days_ahead <= 0:
        days_ahead += 7
    return last + timedelta(days_ahead)

def get_next_weekday(weekday):
    return get_next_weekday_since(weekday, date.today() - timedelta(days=1))

def get_second_next_weekday_since(weekday, last):
    next = get_next_weekday_since(weekday, last)
    return next + timedelta(7)

def get_next_day(day):
    next = date.today() + relativedelta.relativedelta(months=0, day=day)
    if next < date.today():
        next = next + relativedelta.relativedelta(months=1)
    return next