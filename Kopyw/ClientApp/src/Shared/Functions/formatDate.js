const formatDate = date => {
    if (!(date instanceof Date))
        throw new Error("Passed argument is not instance of Date");
    let arr = [date.getDate(), date.getMonth(),
    date.getFullYear(), date.getHours(), date.getMinutes()];
    arr = arr.map(el => el < 10 ? "0" + el : el);
    const [day, month, year, hour, minute] = arr;
    return `${day}.${month}.${year} ${hour}:${minute}`;
}

export default formatDate;