drop database tesla;

create database tesla;

use tesla;

create table user (
    user_id int auto_increment,
    email varchar(255) not null,
    username varchar(200) not null,
    password varchar(200) not null,
    access int not null default 0,
    PRIMARY KEY (user_id)
);

create table car (
    car_id int auto_increment,
    model varchar(50) not null,
    ps int not null,
    acceleration decimal(10,2) not null,
    price decimal(15,2) not null,
    max_range int not null,
    max_speed int not null,
    article_id int not null,
    PRIMARY KEY (car_id)
);

create table orders (
    order_id int auto_increment,
    saldo decimal(15,2) not null,
    order_status int not null,
    article_id int,
    user_id int,
    PRIMARY KEY (order_id),
    FOREIGN KEY (user_id) REFERENCES user (user_id),
    FOREIGN KEY (article_id) REFERENCES car (article_id)
);

INSERT INTO user VALUES(null, "amuehlegger@tsn.at", "alexander", sha2('12345678', 256), 3);
INSERT INTO user VALUES(null, "krillinger@tsn.at", "josef", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "stoani@tsn.at", "stoani", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "domi@tsn.at", "domi", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "simon@tsn.at", "simon", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "marcel@tsn.at", "marcel", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "melih@tsn.at", "melih", sha2('12345678', 256), -1);
INSERT INTO user VALUES(null, "alex@tsn.at", "alex", sha2('12345678', 256), 2);
INSERT INTO user VALUES(null, "domdom@tsn.at", "domdom", sha2('12345678', 256), -2);


INSERT INTO car VALUES(null, "Model S | Standard Range", 541, 3.2, 100000.0, 652, 250, 1);
INSERT INTO car VALUES(null, "Model S | Plaid", 1020, 2.1, 125000.0, 637, 322, 2);

INSERT INTO car VALUES(null, "Model X | Standard Range", 670, 3.9, 105000.0, 560, 250, 3);
INSERT INTO car VALUES(null, "Model X | Plaid", 1020, 2.6, 130000.0, 536, 262, 4);

INSERT INTO car VALUES(null, "Model Y | Long Range", 430, 5.0, 58000.0, 533, 217, 5);
INSERT INTO car VALUES(null, "Model Y | Performance", 513, 3.7, 64000.0, 514, 250, 6);

INSERT INTO car VALUES(null, "Model 3 | Standard Range", 325, 6.1, 55000.0, 491, 225, 7);
INSERT INTO car VALUES(null, "Model 3 | Long Range", 440, 4.4, 59000.0, 602, 233, 8);
INSERT INTO car VALUES(null, "Model 3 | Performance", 510, 3.3, 65000.0, 547, 261, 9);