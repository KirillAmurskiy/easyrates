CREATE TABLE actual_rates
(
    currency_from           text                        NOT NULL,
    currency_to             text                        NOT NULL,
    value                   numeric                     NOT NULL,
    time_of_receipt         timestamp without time zone NOT NULL,
    expiration_time         timestamp without time zone NOT NULL,
    original_published_time timestamp without time zone NOT NULL,
    provider_name           text                        NULL,
    CONSTRAINT pk_actual_rates PRIMARY KEY (currency_from, currency_to)
);

CREATE TABLE rates_history
(
    id                      bigint                      NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    currency_from           text                        NULL,
    currency_to             text                        NULL,
    value                   numeric                     NOT NULL,
    expiration_time         timestamp without time zone NOT NULL,
    original_published_time timestamp without time zone NOT NULL,
    time_of_receipt         timestamp without time zone NOT NULL,
    provider_name           text                        NULL,
    CONSTRAINT pk_rates_history PRIMARY KEY (id)
);
