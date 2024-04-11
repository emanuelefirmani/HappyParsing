namespace HappyParsing;

internal record InputDataObject(
    string Amount,
    string Size
);

internal record DecimalDataObject(
    decimal? Amount,
    decimal? Size,
    decimal? Total,
    string Error
);