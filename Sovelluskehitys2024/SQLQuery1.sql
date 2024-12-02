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

select huolto_id from huollot where rekisteri_nro='SNN-990' and paivamaara='2.12.2024'


/*
CREATE TABLE auton_huoltohistoria (
    rekisteri_nro VARCHAR(50) REFERENCES autot(rekisteri_nro),
    huoltotyyppi VARCHAR(50),
    kilometrit INTEGER,
    paivamaara VARCHAR(50),
    kuva_nimi VARCHAR(100)
);*/


SELECT 
    autot.rekisteri_nro,  
    huollot.huoltotyyppi, 
    huollot.kilometrit, 
    huollot.paivamaara, 
    huoltokuvat.kuva_nimi
FROM huollot
JOIN huoltokuvat ON huollot.huolto_id = huoltokuvat.huolto_id
JOIN autot ON huollot.rekisteri_nro = autot.rekisteri_nro
WHERE autot.rekisteri_nro = 'SNN-990';  



/*
VARBINARY(MAX)

select * from huollot

Drop table auton_huoltohistoria

Drop table asiakkaat

drop table tuotteet

drop table tilaukset














CREATE TABLE tuotteet (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50), hinta INTEGER);

CREATE TABLE asiakkaat (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50), osoite VARCHAR(150), puhelin VARCHAR(50));

CREATE TABLE tilaukset (id INTEGER IDENTITY(1,1) PRIMARY KEY, asiakas_id INTEGER REFERENCES asiakkaat ON DELETE CASCADE, tuote_id INTEGER REFERENCES tuotteet ON DELETE CASCADE, toimitettu BIT DEFAULT 0);

INSERT INTO tuotteet (nimi, hinta) VALUES ('juusto', 6);
INSERT INTO asiakkaat (nimi, osoite, puhelin) VALUES ('Masa', 'Kuusikuja 6', '050882682');
INSERT INTO tilaukset (asiakas_id, tuote_id) VALUES (1,1); 

DELETE FROM tuotteet WHERE id=5;

SELECT * FROM tuotteet;
SELECT * FROM asiakkaat;
SELECT * FROM tilaukset;

UPDATE tilaukset SET toimitettu=1 WHERE id=1

SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id

DELETE FROM tuotteet WHERE nimi="kinkku";

DROP TABLE tilaukset;
*/