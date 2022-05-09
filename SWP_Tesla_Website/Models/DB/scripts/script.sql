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
    PRIMARY KEY (car_id)
);

INSERT INTO user VALUES(null, "amuehlegger@tsn.at", "alexander", sha2('12345678', 256), 3);
INSERT INTO user VALUES(null, "krillinger@tsn.at", "josef", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "stoani@tsn.at", "stoani", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "domi@tsn.at", "domi", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "simon@tsn.at", "simon", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "marcel@tsn.at", "marcel", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "melih@tsn.at", "melih", sha2('12345678', 256), -1);
INSERT INTO user VALUES(null, "alex@tsn.at", "alex", sha2('12345678', 256), 2);
INSERT INTO user VALUES(null, "dodmdom@tsn.at", "domdom", sha2('12345678', 256), -2);
INSERT INTO user VALUES(null, "dofdmi@tsn.at", "dodfmi", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "simdfon@tsn.at", "simofdn", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "marcdfel@tsn.at", "mardfcel", sha2('12345678', 256), DEFAULT);
INSERT INTO user VALUES(null, "meladih@tsn.at", "melfdih", sha2('12345678', 256), -1);
INSERT INTO user VALUES(null, "aldfex@tsn.at", "aledfx", sha2('12345678', 256), 2);
INSERT INTO user VALUES(null, "domfddom@tsn.at", "dafomdom", sha2('12345678', 256), -2);



INSERT INTO car VALUES(null, "Model S | Standard Range", 541, 3.2, 100000.0, 652, 250);
INSERT INTO car VALUES(null, "Model S | Plaid", 1020, 2.1, 125000.0, 637, 322);

INSERT INTO car VALUES(null, "Model X | Standard Range", 670, 3.9, 105000.0, 560, 250);
INSERT INTO car VALUES(null, "Model X | Plaid", 1020, 2.6, 130000.0, 536, 262);

INSERT INTO car VALUES(null, "Model Y | Long Range", 430, 5.0, 58000.0, 533, 217);
INSERT INTO car VALUES(null, "Model Y | Performance", 513, 3.7, 64000.0, 514, 250);

INSERT INTO car VALUES(null, "Model 3 | Standard Range", 325, 6.1, 55000.0, 491, 225);
INSERT INTO car VALUES(null, "Model 3 | Long Range", 440, 4.4, 59000.0, 602, 233);
INSERT INTO car VALUES(null, "Model 3 | Performance", 510, 3.3, 65000.0, 547, 261);