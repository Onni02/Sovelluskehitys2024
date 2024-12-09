CREATE TABLE autot (
    auto_id INTEGER IDENTITY(1,1) PRIMARY KEY,
    merkki VARCHAR(50),
    malli VARCHAR(50),
    rekisteri_nro VARCHAR(50) UNIQUE
);

CREATE TABLE huollot (
    huolto_id INTEGER IDENTITY(1,1) PRIMARY KEY,
    rekisteri_nro VARCHAR(50) REFERENCES autot(rekisteri_nro) ON DELETE CASCADE,
    huoltotyyppi VARCHAR(50),
    kilometrit INTEGER,
    paivamaara VARCHAR(50)
);

CREATE TABLE huoltokuvat (
    kuva_id INTEGER IDENTITY(1,1) PRIMARY KEY,
    huolto_id INTEGER REFERENCES huollot(huolto_id) ON DELETE CASCADE,
    kuva VARCHAR(400),
    kuva_nimi VARCHAR(100)
);



CREATE TABLE omistaja (
    nimi VARCHAR(80),
    puhelinnumero VARCHAR(25),
    osoite VARCHAR(200),
    rekisteri_nro VARCHAR(50) REFERENCES autot(rekisteri_nro) ON DELETE CASCADE
);






