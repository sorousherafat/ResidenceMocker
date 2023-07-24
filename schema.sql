CREATE TYPE bed_type AS ENUM (
    'king',
    'queen',
    'double',
    'single'
);

CREATE TYPE building_type AS ENUM (
    'apartment',
    'villa',
    'hotel',
    'eco'
);

CREATE TYPE rent_status AS ENUM (
    'cancelled',
    'pending',
    'completed',
    'ongoing'
);

CREATE TYPE rental_request_status AS ENUM (
    'pending',
    'cancelled',
    'rejected',
    'accepted'
);

CREATE TABLE account (
    nationalid character(10) PRIMARY KEY CHECK (nationalid ~ '[0-9]{10}'),
    firstname character varying(20) CHECK (length(firstname) > 0),
    lastname character varying(20) CHECK (length(lastname) > 0),
    phonenumber character(11) UNIQUE CHECK (phonenumber ~ '09[0-9]{9}'),
    joinedat timestamp without time zone NOT NULL
);

CREATE TABLE host (
    nationalid character(10) PRIMARY KEY REFERENCES account (
        nationalid
    ) ON DELETE CASCADE,
    nationalcardimage bytea,
    isverified boolean DEFAULT false,
    verifiedat timestamp without time zone
);

CREATE TABLE guest (
    nationalid character(10) PRIMARY KEY REFERENCES account (
        nationalid
    ) ON DELETE CASCADE,
    wallet integer DEFAULT 0
);

CREATE TABLE province (
    id serial PRIMARY KEY,
    name character varying(20) CHECK (name ~ '[A-Za-z]+')
);

CREATE TABLE city (
    id serial PRIMARY KEY,
    provinceid integer NOT NULL REFERENCES province (id) ON DELETE CASCADE,
    name character varying(20) CHECK (name ~ '[A-Za-z]+')
);

CREATE TABLE address (
    id serial PRIMARY KEY,
    cityid integer NOT NULL REFERENCES city (id),
    street character varying(20) NOT NULL CHECK (length(street) > 0),
    details character varying(200) NOT NULL CHECK (length(details) > 0),
    zipcode character(10) CHECK (zipcode ~ '[0-9]{10}'),
    geolocation point,
    isrural boolean
);

CREATE TABLE residence (
    id serial PRIMARY KEY,
    addressid integer NOT NULL REFERENCES address (id),
    hostnationalid character(10) NOT NULL REFERENCES host (
        nationalid
    ) ON DELETE CASCADE,
    primaryphoto bytea,
    allphotos bytea [],
    price integer CHECK (price >= 0),
    rentfee integer DEFAULT 0,
    area numeric(6, 2) NOT NULL CHECK (area > 0),
    roomsno smallint DEFAULT 0,
    capacity smallint DEFAULT 1,
    bedstype bed_type [],
    facility text,
    checkintime time without time zone NOT NULL,
    checkouttime time without time zone NOT NULL,
    cancellationpolicy jsonb NOT NULL,
    createdat timestamp without time zone NOT NULL,
    title character varying(20),
    haswifi boolean DEFAULT false,
    hasparking boolean DEFAULT false,
    buildingtype building_type NOT NULL
);

CREATE TABLE rentalrequest (
    id serial PRIMARY KEY,
    residenceid integer REFERENCES residence (id) ON DELETE SET NULL,
    guestnationalid character(10) REFERENCES guest (
        nationalid
    ) ON DELETE SET NULL,
    startdate date NOT NULL,
    enddate date NOT NULL,
    guestsno smallint CHECK (guestsno > 0),
    rawprice integer CHECK (rawprice >= 0),
    status rental_request_status NOT NULL,
    createdat timestamp without time zone NOT NULL,
    resolvedat timestamp without time zone
);

CREATE TABLE rent (
    id serial PRIMARY KEY REFERENCES rentalrequest (id) ON DELETE CASCADE,
    cancellationpolicy jsonb NOT NULL,
    cancellationpenalty integer,
    cancellationtimestamp timestamp without time zone,
    finalprice integer NOT NULL,
    status rent_status NOT NULL
);

CREATE TABLE complaint (
    id serial NOT NULL,
    rentid integer NOT NULL REFERENCES rent (id) ON DELETE CASCADE,
    isaccepted boolean DEFAULT false,
    title character varying(40),
    description text NOT NULL,
    photos bytea [],
    createdat timestamp without time zone NOT NULL,
    resolvedat timestamp without time zone,
    PRIMARY KEY (id, rentid)
);

CREATE TABLE damagereport (
    id serial NOT NULL,
    rentid integer NOT NULL REFERENCES rent (id) ON DELETE CASCADE,
    estimatedcost integer NOT NULL,
    isaccepted boolean DEFAULT false,
    title character varying(40),
    description text NOT NULL,
    photos bytea [],
    finalcost integer,
    createdat timestamp without time zone NOT NULL,
    resolvedat timestamp without time zone,
    PRIMARY KEY (id, rentid)
);

CREATE TABLE message (
    id serial PRIMARY KEY,
    text text NOT NULL,
    sentat timestamp without time zone NOT NULL,
    guestnationalid character(10) REFERENCES guest (
        nationalid
    ) ON DELETE CASCADE,
    hostnationalid character(10) REFERENCES host (nationalid) ON DELETE CASCADE,
    sentbyhost boolean DEFAULT false NOT NULL
);

CREATE TABLE pricechange (
    id serial NOT NULL,
    residenceid integer NOT NULL REFERENCES residence (id) ON DELETE CASCADE,
    factor numeric(4, 2),
    createdat timestamp without time zone NOT NULL,
    starttime timestamp without time zone,
    endtime timestamp without time zone,
    PRIMARY KEY (id, residenceid)
);

CREATE TABLE review (
    id serial PRIMARY KEY,
    isbyhost boolean NOT NULL,
    rentid integer REFERENCES rent (id) ON DELETE CASCADE,
    rating smallint,
    comment text,
    createdat timestamp without time zone NOT NULL,
    CONSTRAINT review_rating_check CHECK (
        (rating = any(ARRAY[null::integer, 0, 1, 2, 3, 4, 5]))
    )
);

CREATE TABLE unavailability (
    id serial NOT NULL,
    residenceid integer NOT NULL REFERENCES residence (id) ON DELETE CASCADE,
    createdat timestamp without time zone NOT NULL,
    starttime timestamp without time zone,
    endtime timestamp without time zone,
    PRIMARY KEY (id, residenceid)
);
