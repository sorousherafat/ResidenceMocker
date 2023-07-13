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
    nationalid character(10) NOT NULL,
    firstname character varying(20),
    lastname character varying(20),
    phonenumber character(11),
    joinedat timestamp without time zone NOT NULL,
    CONSTRAINT account_firstname_check CHECK ((length((firstname)::text) > 0)),
    CONSTRAINT account_lastname_check CHECK ((length((lastname)::text) > 0)),
    CONSTRAINT account_nationalid_check CHECK ((nationalid ~ similar_to_escape('[0-9]{10}'::text))),
    CONSTRAINT account_phonenumber_check CHECK ((phonenumber ~ similar_to_escape('09[0-9]{9}'::text)))
);

CREATE TABLE address (
    id serial NOT NULL,
    cityid integer NOT NULL,
    street character varying(20) NOT NULL,
    details character varying(200) NOT NULL,
    zipcode character(10),
    geolocation point,
    isrural boolean,
    CONSTRAINT address_details_check CHECK ((length((details)::text) > 0)),
    CONSTRAINT address_street_check CHECK ((length((street)::text) > 0)),
    CONSTRAINT address_zipcode_check CHECK ((zipcode ~ similar_to_escape('[0-9]{10}'::text)))
);

CREATE TABLE city (
    id serial NOT NULL,
    provinceid integer NOT NULL,
    name character varying(20),
    CONSTRAINT city_name_check CHECK (((name)::text ~ similar_to_escape('[A-Za-z]+'::text)))
);

CREATE TABLE complaint (
    id serial NOT NULL,
    rentid integer NOT NULL,
    isaccepted boolean DEFAULT false,
    title character varying(40),
    description text NOT NULL,
    photos bytea[],
    createdat timestamp without time zone NOT NULL,
    resolvedat timestamp without time zone
);

CREATE TABLE damagereport (
    id serial NOT NULL,
    rentid integer NOT NULL,
    estimatedcost integer NOT NULL,
    isaccepted boolean DEFAULT false,
    title character varying(40),
    description text NOT NULL,
    photos bytea[],
    finalcost integer,
    createdat timestamp without time zone NOT NULL,
    resolvedat timestamp without time zone
);

CREATE TABLE guest (
    nationalid character(10) NOT NULL,
    wallet integer DEFAULT 0
);

CREATE TABLE host (
    nationalid character(10) NOT NULL,
    nationalcardimage bytea,
    isverified boolean DEFAULT false,
    verifiedat timestamp without time zone
);

CREATE TABLE message (
    id serial NOT NULL,
    sendernationalid character(10),
    recievernationalid character(10),
    text text NOT NULL,
    sentat timestamp without time zone NOT NULL
);

CREATE TABLE pricechange (
    id serial NOT NULL,
    residenceid integer NOT NULL,
    factor numeric(4,2),
    createdat timestamp without time zone NOT NULL,
    starttime timestamp without time zone,
    endtime timestamp without time zone
);

CREATE TABLE province (
    id serial NOT NULL,
    name character varying(20),
    CONSTRAINT province_name_check CHECK (((name)::text ~ similar_to_escape('[A-Za-z]+'::text)))
);

CREATE TABLE rent (
    id serial NOT NULL,
    cancellationpolicy jsonb NOT NULL,
    cancellationpenalty integer,
    cancellationtimestamp timestamp without time zone,
    finalprice integer NOT NULL,
    status rent_status NOT NULL
);

CREATE TABLE rentalrequest (
    id serial NOT NULL,
    residenceid integer,
    guestnationalid character(10),
    startdate date NOT NULL,
    enddate date NOT NULL,
    guestsno smallint,
    rawprice integer,
    status rental_request_status NOT NULL,
    createdat timestamp without time zone NOT NULL,
    resolvedat timestamp without time zone,
    CONSTRAINT rentalrequest_guestsno_check CHECK ((guestsno > 0)),
    CONSTRAINT rentalrequest_rawprice_check CHECK ((rawprice >= 0))
);

CREATE TABLE residence (
    id serial NOT NULL,
    addressid integer NOT NULL,
    hostnationalid character(10) NOT NULL,
    primaryphoto bytea,
    allphotos bytea[],
    price integer,
    rentfee integer DEFAULT 0,
    area numeric(6,2) NOT NULL,
    roomsno smallint DEFAULT 0,
    capacity smallint DEFAULT 1,
    bedstype bed_type[],
    facility text,
    checkintime time without time zone NOT NULL,
    checkouttime time without time zone NOT NULL,
    cancellationpolicy jsonb NOT NULL,
    createdat timestamp without time zone NOT NULL,
    title character varying(20),
    haswifi boolean DEFAULT false,
    hasparking boolean DEFAULT false,
    buildingtype building_type NOT NULL,
    CONSTRAINT residence_area_check CHECK ((area > (0)::numeric)),
    CONSTRAINT residence_price_check CHECK ((price >= 0)),
    CONSTRAINT residence_price_check1 CHECK ((price >= 0))
);

CREATE TABLE review (
    id serial NOT NULL,
    isbyhost boolean NOT NULL,
    rentid integer,
    rating smallint,
    comment text,
    createdat timestamp without time zone NOT NULL,
    CONSTRAINT review_rating_check CHECK ((rating = ANY (ARRAY[NULL::integer, 0, 1, 2, 3, 4, 5])))
);

CREATE TABLE unavailability (
    id serial NOT NULL,
    residenceid integer NOT NULL,
    createdat timestamp without time zone NOT NULL,
    starttime timestamp without time zone,
    endtime timestamp without time zone
);

ALTER TABLE ONLY account
    ADD CONSTRAINT account_phonenumber_key UNIQUE (phonenumber);

ALTER TABLE ONLY account
    ADD CONSTRAINT account_pkey PRIMARY KEY (nationalid);

ALTER TABLE ONLY address
    ADD CONSTRAINT address_pkey PRIMARY KEY (id);

ALTER TABLE ONLY city
    ADD CONSTRAINT city_pkey PRIMARY KEY (id);

ALTER TABLE ONLY complaint
    ADD CONSTRAINT complaint_pkey PRIMARY KEY (id, rentid);

ALTER TABLE ONLY damagereport
    ADD CONSTRAINT damagereport_pkey PRIMARY KEY (id, rentid);

ALTER TABLE ONLY guest
    ADD CONSTRAINT guest_pkey PRIMARY KEY (nationalid);

ALTER TABLE ONLY host
    ADD CONSTRAINT host_pkey PRIMARY KEY (nationalid);

ALTER TABLE ONLY message
    ADD CONSTRAINT message_pkey PRIMARY KEY (id);

ALTER TABLE ONLY pricechange
    ADD CONSTRAINT pricechange_pkey PRIMARY KEY (id, residenceid);

ALTER TABLE ONLY province
    ADD CONSTRAINT province_pkey PRIMARY KEY (id);

ALTER TABLE ONLY rent
    ADD CONSTRAINT rent_pkey PRIMARY KEY (id);

ALTER TABLE ONLY rentalrequest
    ADD CONSTRAINT rentalrequest_pkey PRIMARY KEY (id);

ALTER TABLE ONLY residence
    ADD CONSTRAINT residence_pkey PRIMARY KEY (id);

ALTER TABLE ONLY review
    ADD CONSTRAINT review_pkey PRIMARY KEY (id);

ALTER TABLE ONLY unavailability
    ADD CONSTRAINT unavailablity_pkey PRIMARY KEY (id, residenceid);

ALTER TABLE ONLY address
    ADD CONSTRAINT address_cityid_fkey FOREIGN KEY (cityid) REFERENCES city(id);

ALTER TABLE ONLY city
    ADD CONSTRAINT city_provinceid_fkey FOREIGN KEY (provinceid) REFERENCES province(id) ON DELETE CASCADE;

ALTER TABLE ONLY complaint
    ADD CONSTRAINT complaint_rentid_fkey FOREIGN KEY (rentid) REFERENCES rent(id) ON DELETE CASCADE;

ALTER TABLE ONLY damagereport
    ADD CONSTRAINT damagereport_rentid_fkey FOREIGN KEY (rentid) REFERENCES rent(id) ON DELETE CASCADE;

ALTER TABLE ONLY guest
    ADD CONSTRAINT guest_nationalid_fkey FOREIGN KEY (nationalid) REFERENCES account(nationalid) ON DELETE CASCADE;

ALTER TABLE ONLY host
    ADD CONSTRAINT host_nationalid_fkey FOREIGN KEY (nationalid) REFERENCES account(nationalid) ON DELETE CASCADE;

ALTER TABLE ONLY message
    ADD CONSTRAINT message_recievernationalid_fkey FOREIGN KEY (recievernationalid) REFERENCES account(nationalid) ON DELETE SET NULL;

ALTER TABLE ONLY message
    ADD CONSTRAINT message_sendernationalid_fkey FOREIGN KEY (sendernationalid) REFERENCES account(nationalid) ON DELETE SET NULL;

ALTER TABLE ONLY pricechange
    ADD CONSTRAINT pricechange_residenceid_fkey FOREIGN KEY (residenceid) REFERENCES residence(id) ON DELETE CASCADE;

ALTER TABLE ONLY rent
    ADD CONSTRAINT rent_id_fkey FOREIGN KEY (id) REFERENCES rentalrequest(id) ON DELETE CASCADE;

ALTER TABLE ONLY rentalrequest
    ADD CONSTRAINT rentalrequest_guestnationalid_fkey FOREIGN KEY (guestnationalid) REFERENCES guest(nationalid) ON DELETE SET NULL;

ALTER TABLE ONLY rentalrequest
    ADD CONSTRAINT rentalrequest_residenceid_fkey FOREIGN KEY (residenceid) REFERENCES residence(id) ON DELETE SET NULL;

ALTER TABLE ONLY residence
    ADD CONSTRAINT residence_addressid_fkey FOREIGN KEY (addressid) REFERENCES address(id);

ALTER TABLE ONLY residence
    ADD CONSTRAINT residence_hostnationalid_fkey FOREIGN KEY (hostnationalid) REFERENCES host(nationalid) ON DELETE CASCADE;

ALTER TABLE ONLY review
    ADD CONSTRAINT review_rentid_fkey FOREIGN KEY (rentid) REFERENCES rent(id) ON DELETE CASCADE;

ALTER TABLE ONLY unavailability
    ADD CONSTRAINT unavailablity_residenceid_fkey FOREIGN KEY (residenceid) REFERENCES residence(id) ON DELETE CASCADE;
