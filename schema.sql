CREATE TYPE public.bed_type AS ENUM (
    'king',
    'queen',
    'double',
    'single'
    );


ALTER TYPE public.bed_type OWNER TO freak;

--
-- Name: building_type; Type: TYPE; Schema: public; Owner: freak
--

CREATE TYPE public.building_type AS ENUM (
    'apartment',
    'villa',
    'hotel',
    'eco'
    );


ALTER TYPE public.building_type OWNER TO freak;

--
-- Name: rent_status; Type: TYPE; Schema: public; Owner: freak
--

CREATE TYPE public.rent_status AS ENUM (
    'cancelled',
    'pending',
    'completed',
    'ongoing'
    );


ALTER TYPE public.rent_status OWNER TO freak;

--
-- Name: rental_request_status; Type: TYPE; Schema: public; Owner: freak
--

CREATE TYPE public.rental_request_status AS ENUM (
    'pending',
    'cancelled',
    'rejected',
    'accepted'
    );


ALTER TYPE public.rental_request_status OWNER TO freak;

--
-- Name: check_rent_conflict(); Type: FUNCTION; Schema: public; Owner: freak
--

CREATE FUNCTION public.check_rent_conflict() RETURNS trigger
    LANGUAGE plpgsql
AS $$
BEGIN
    IF EXISTS (
        SELECT *
        FROM RentalRequest
        WHERE ResidenceID = (SELECT ResidenceID FROM RentalRequest WHERE ID = NEW.ID)
          AND Status = 'accepted'
          AND ID <> NEW.ID
          AND (StartDate, EndDate) OVERLAPS ((SELECT StartDate FROM RentalRequest WHERE ID = NEW.ID), (SELECT EndDate FROM RentalRequest WHERE ID = NEW.ID))
    ) THEN
        RAISE EXCEPTION 'Conflict with another rent.';
    END IF;

    RETURN NEW;
END;
$$;


ALTER FUNCTION public.check_rent_conflict() OWNER TO freak;

--
-- Name: truncate_tables(character varying); Type: FUNCTION; Schema: public; Owner: freak
--

CREATE FUNCTION public.truncate_tables(username character varying) RETURNS void
    LANGUAGE plpgsql
AS $$
DECLARE
    statements CURSOR FOR
        SELECT tablename FROM pg_tables
        WHERE tableowner = username AND schemaname = 'public';
BEGIN
    FOR stmt IN statements LOOP
            EXECUTE 'TRUNCATE TABLE ' || quote_ident(stmt.tablename) || ' CASCADE;';
        END LOOP;
END;
$$;


ALTER FUNCTION public.truncate_tables(username character varying) OWNER TO freak;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: account; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.account (
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


ALTER TABLE public.account OWNER TO freak;

--
-- Name: address; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.address (
                                id integer NOT NULL,
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


ALTER TABLE public.address OWNER TO freak;

--
-- Name: address_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.address_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.address_id_seq OWNER TO freak;

--
-- Name: address_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.address_id_seq OWNED BY public.address.id;


--
-- Name: city; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.city (
                             id integer NOT NULL,
                             provinceid integer NOT NULL,
                             name character varying(20),
                             CONSTRAINT city_name_check CHECK (((name)::text ~ similar_to_escape('[A-Za-z]+'::text)))
);


ALTER TABLE public.city OWNER TO freak;

--
-- Name: city_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.city_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.city_id_seq OWNER TO freak;

--
-- Name: city_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.city_id_seq OWNED BY public.city.id;


--
-- Name: complaint; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.complaint (
                                  id integer NOT NULL,
                                  rentid integer NOT NULL,
                                  isaccepted boolean DEFAULT false,
                                  title character varying(40),
                                  description text NOT NULL,
                                  photos bytea[],
                                  createdat timestamp without time zone NOT NULL,
                                  resolvedat timestamp without time zone
);


ALTER TABLE public.complaint OWNER TO freak;

--
-- Name: complaint_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.complaint_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.complaint_id_seq OWNER TO freak;

--
-- Name: complaint_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.complaint_id_seq OWNED BY public.complaint.id;


--
-- Name: damagereport; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.damagereport (
                                     id integer NOT NULL,
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


ALTER TABLE public.damagereport OWNER TO freak;

--
-- Name: damagereport_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.damagereport_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.damagereport_id_seq OWNER TO freak;

--
-- Name: damagereport_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.damagereport_id_seq OWNED BY public.damagereport.id;


--
-- Name: guest; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.guest (
                              nationalid character(10) NOT NULL,
                              wallet integer DEFAULT 0
);


ALTER TABLE public.guest OWNER TO freak;

--
-- Name: host; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.host (
                             nationalid character(10) NOT NULL,
                             nationalcardimage bytea,
                             isverified boolean DEFAULT false,
                             verifiedat timestamp without time zone
);


ALTER TABLE public.host OWNER TO freak;

--
-- Name: message; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.message (
                                id integer NOT NULL,
                                text text NOT NULL,
                                sentat timestamp without time zone NOT NULL,
                                guestnationalid character(10),
                                hostnationalid character(10),
                                sentbyhost boolean DEFAULT false NOT NULL
);


ALTER TABLE public.message OWNER TO freak;

--
-- Name: message_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.message_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.message_id_seq OWNER TO freak;

--
-- Name: message_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.message_id_seq OWNED BY public.message.id;


--
-- Name: pricechange; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.pricechange (
                                    id integer NOT NULL,
                                    residenceid integer NOT NULL,
                                    factor numeric(4,2),
                                    createdat timestamp without time zone NOT NULL,
                                    starttime timestamp without time zone,
                                    endtime timestamp without time zone
);


ALTER TABLE public.pricechange OWNER TO freak;

--
-- Name: pricechange_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.pricechange_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.pricechange_id_seq OWNER TO freak;

--
-- Name: pricechange_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.pricechange_id_seq OWNED BY public.pricechange.id;


--
-- Name: province; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.province (
                                 id integer NOT NULL,
                                 name character varying(20),
                                 CONSTRAINT province_name_check CHECK (((name)::text ~ similar_to_escape('[A-Za-z]+'::text)))
);


ALTER TABLE public.province OWNER TO freak;

--
-- Name: province_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.province_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.province_id_seq OWNER TO freak;

--
-- Name: province_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.province_id_seq OWNED BY public.province.id;


--
-- Name: rent; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.rent (
                             id integer NOT NULL,
                             cancellationpolicy jsonb NOT NULL,
                             cancellationpenalty integer,
                             cancellationtimestamp timestamp without time zone,
                             finalprice integer NOT NULL,
                             status public.rent_status NOT NULL
);


ALTER TABLE public.rent OWNER TO freak;

--
-- Name: rent_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.rent_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.rent_id_seq OWNER TO freak;

--
-- Name: rent_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.rent_id_seq OWNED BY public.rent.id;


--
-- Name: rentalrequest; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.rentalrequest (
                                      id integer NOT NULL,
                                      residenceid integer,
                                      guestnationalid character(10),
                                      startdate date NOT NULL,
                                      enddate date NOT NULL,
                                      guestsno smallint,
                                      rawprice integer,
                                      status public.rental_request_status NOT NULL,
                                      createdat timestamp without time zone NOT NULL,
                                      resolvedat timestamp without time zone,
                                      CONSTRAINT rentalrequest_guestsno_check CHECK ((guestsno > 0)),
                                      CONSTRAINT rentalrequest_rawprice_check CHECK ((rawprice >= 0))
);


ALTER TABLE public.rentalrequest OWNER TO freak;

--
-- Name: rentalrequest_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.rentalrequest_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.rentalrequest_id_seq OWNER TO freak;

--
-- Name: rentalrequest_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.rentalrequest_id_seq OWNED BY public.rentalrequest.id;


--
-- Name: residence; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.residence (
                                  id integer NOT NULL,
                                  addressid integer NOT NULL,
                                  hostnationalid character(10) NOT NULL,
                                  primaryphoto bytea,
                                  allphotos bytea[],
                                  price integer,
                                  rentfee integer DEFAULT 0,
                                  area numeric(6,2) NOT NULL,
                                  roomsno smallint DEFAULT 0,
                                  capacity smallint DEFAULT 1,
                                  bedstype public.bed_type[],
                                  facility text,
                                  checkintime time without time zone NOT NULL,
                                  checkouttime time without time zone NOT NULL,
                                  cancellationpolicy jsonb NOT NULL,
                                  createdat timestamp without time zone NOT NULL,
                                  title character varying(20),
                                  haswifi boolean DEFAULT false,
                                  hasparking boolean DEFAULT false,
                                  buildingtype public.building_type NOT NULL,
                                  CONSTRAINT residence_area_check CHECK ((area > (0)::numeric)),
                                  CONSTRAINT residence_price_check CHECK ((price >= 0)),
                                  CONSTRAINT residence_price_check1 CHECK ((price >= 0))
);


ALTER TABLE public.residence OWNER TO freak;

--
-- Name: residence_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.residence_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.residence_id_seq OWNER TO freak;

--
-- Name: residence_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.residence_id_seq OWNED BY public.residence.id;


--
-- Name: review; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.review (
                               id integer NOT NULL,
                               isbyhost boolean NOT NULL,
                               rentid integer,
                               rating smallint,
                               comment text,
                               createdat timestamp without time zone NOT NULL,
                               CONSTRAINT review_rating_check CHECK ((rating = ANY (ARRAY[NULL::integer, 0, 1, 2, 3, 4, 5])))
);


ALTER TABLE public.review OWNER TO freak;

--
-- Name: review_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.review_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.review_id_seq OWNER TO freak;

--
-- Name: review_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.review_id_seq OWNED BY public.review.id;


--
-- Name: unavailability; Type: TABLE; Schema: public; Owner: freak
--

CREATE TABLE public.unavailability (
                                       id integer NOT NULL,
                                       residenceid integer NOT NULL,
                                       createdat timestamp without time zone NOT NULL,
                                       starttime timestamp without time zone,
                                       endtime timestamp without time zone
);


ALTER TABLE public.unavailability OWNER TO freak;

--
-- Name: unavailability_id_seq; Type: SEQUENCE; Schema: public; Owner: freak
--

CREATE SEQUENCE public.unavailability_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.unavailability_id_seq OWNER TO freak;

--
-- Name: unavailability_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: freak
--

ALTER SEQUENCE public.unavailability_id_seq OWNED BY public.unavailability.id;


--
-- Name: address id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.address ALTER COLUMN id SET DEFAULT nextval('public.address_id_seq'::regclass);


--
-- Name: city id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.city ALTER COLUMN id SET DEFAULT nextval('public.city_id_seq'::regclass);


--
-- Name: complaint id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.complaint ALTER COLUMN id SET DEFAULT nextval('public.complaint_id_seq'::regclass);


--
-- Name: damagereport id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.damagereport ALTER COLUMN id SET DEFAULT nextval('public.damagereport_id_seq'::regclass);


--
-- Name: message id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.message ALTER COLUMN id SET DEFAULT nextval('public.message_id_seq'::regclass);


--
-- Name: pricechange id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.pricechange ALTER COLUMN id SET DEFAULT nextval('public.pricechange_id_seq'::regclass);


--
-- Name: province id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.province ALTER COLUMN id SET DEFAULT nextval('public.province_id_seq'::regclass);


--
-- Name: rent id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.rent ALTER COLUMN id SET DEFAULT nextval('public.rent_id_seq'::regclass);


--
-- Name: rentalrequest id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.rentalrequest ALTER COLUMN id SET DEFAULT nextval('public.rentalrequest_id_seq'::regclass);


--
-- Name: residence id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.residence ALTER COLUMN id SET DEFAULT nextval('public.residence_id_seq'::regclass);


--
-- Name: review id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.review ALTER COLUMN id SET DEFAULT nextval('public.review_id_seq'::regclass);


--
-- Name: unavailability id; Type: DEFAULT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.unavailability ALTER COLUMN id SET DEFAULT nextval('public.unavailability_id_seq'::regclass);


--
-- Name: account account_phonenumber_key; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.account
    ADD CONSTRAINT account_phonenumber_key UNIQUE (phonenumber);


--
-- Name: account account_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.account
    ADD CONSTRAINT account_pkey PRIMARY KEY (nationalid);


--
-- Name: address address_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.address
    ADD CONSTRAINT address_pkey PRIMARY KEY (id);


--
-- Name: city city_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.city
    ADD CONSTRAINT city_pkey PRIMARY KEY (id);


--
-- Name: complaint complaint_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.complaint
    ADD CONSTRAINT complaint_pkey PRIMARY KEY (id, rentid);


--
-- Name: damagereport damagereport_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.damagereport
    ADD CONSTRAINT damagereport_pkey PRIMARY KEY (id, rentid);


--
-- Name: guest guest_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.guest
    ADD CONSTRAINT guest_pkey PRIMARY KEY (nationalid);


--
-- Name: host host_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.host
    ADD CONSTRAINT host_pkey PRIMARY KEY (nationalid);


--
-- Name: message message_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.message
    ADD CONSTRAINT message_pkey PRIMARY KEY (id);


--
-- Name: pricechange pricechange_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.pricechange
    ADD CONSTRAINT pricechange_pkey PRIMARY KEY (id, residenceid);


--
-- Name: province province_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.province
    ADD CONSTRAINT province_pkey PRIMARY KEY (id);


--
-- Name: rent rent_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.rent
    ADD CONSTRAINT rent_pkey PRIMARY KEY (id);


--
-- Name: rentalrequest rentalrequest_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.rentalrequest
    ADD CONSTRAINT rentalrequest_pkey PRIMARY KEY (id);


--
-- Name: residence residence_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.residence
    ADD CONSTRAINT residence_pkey PRIMARY KEY (id);


--
-- Name: review review_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT review_pkey PRIMARY KEY (id);


--
-- Name: unavailability unavailablity_pkey; Type: CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.unavailability
    ADD CONSTRAINT unavailablity_pkey PRIMARY KEY (id, residenceid);


--
-- Name: rent check_rent_trigger; Type: TRIGGER; Schema: public; Owner: freak
--

CREATE TRIGGER check_rent_trigger BEFORE INSERT OR UPDATE ON public.rent FOR EACH ROW EXECUTE FUNCTION public.check_rent_conflict();


--
-- Name: address address_cityid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.address
    ADD CONSTRAINT address_cityid_fkey FOREIGN KEY (cityid) REFERENCES public.city(id);


--
-- Name: city city_provinceid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.city
    ADD CONSTRAINT city_provinceid_fkey FOREIGN KEY (provinceid) REFERENCES public.province(id) ON DELETE CASCADE;


--
-- Name: complaint complaint_rentid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.complaint
    ADD CONSTRAINT complaint_rentid_fkey FOREIGN KEY (rentid) REFERENCES public.rent(id) ON DELETE CASCADE;


--
-- Name: damagereport damagereport_rentid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.damagereport
    ADD CONSTRAINT damagereport_rentid_fkey FOREIGN KEY (rentid) REFERENCES public.rent(id) ON DELETE CASCADE;


--
-- Name: guest guest_nationalid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.guest
    ADD CONSTRAINT guest_nationalid_fkey FOREIGN KEY (nationalid) REFERENCES public.account(nationalid) ON DELETE CASCADE;


--
-- Name: host host_nationalid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.host
    ADD CONSTRAINT host_nationalid_fkey FOREIGN KEY (nationalid) REFERENCES public.account(nationalid) ON DELETE CASCADE;


--
-- Name: message message_guestnationalid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.message
    ADD CONSTRAINT message_guestnationalid_fkey FOREIGN KEY (guestnationalid) REFERENCES public.guest(nationalid) ON DELETE CASCADE;


--
-- Name: message message_hostnationalid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.message
    ADD CONSTRAINT message_hostnationalid_fkey FOREIGN KEY (hostnationalid) REFERENCES public.host(nationalid) ON DELETE CASCADE;


--
-- Name: pricechange pricechange_residenceid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.pricechange
    ADD CONSTRAINT pricechange_residenceid_fkey FOREIGN KEY (residenceid) REFERENCES public.residence(id) ON DELETE CASCADE;


--
-- Name: rent rent_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.rent
    ADD CONSTRAINT rent_id_fkey FOREIGN KEY (id) REFERENCES public.rentalrequest(id) ON DELETE CASCADE;


--
-- Name: rentalrequest rentalrequest_guestnationalid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.rentalrequest
    ADD CONSTRAINT rentalrequest_guestnationalid_fkey FOREIGN KEY (guestnationalid) REFERENCES public.guest(nationalid) ON DELETE SET NULL;


--
-- Name: rentalrequest rentalrequest_residenceid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.rentalrequest
    ADD CONSTRAINT rentalrequest_residenceid_fkey FOREIGN KEY (residenceid) REFERENCES public.residence(id) ON DELETE SET NULL;


--
-- Name: residence residence_addressid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.residence
    ADD CONSTRAINT residence_addressid_fkey FOREIGN KEY (addressid) REFERENCES public.address(id);


--
-- Name: residence residence_hostnationalid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.residence
    ADD CONSTRAINT residence_hostnationalid_fkey FOREIGN KEY (hostnationalid) REFERENCES public.host(nationalid) ON DELETE CASCADE;


--
-- Name: review review_rentid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT review_rentid_fkey FOREIGN KEY (rentid) REFERENCES public.rent(id) ON DELETE CASCADE;


--
-- Name: unavailability unavailablity_residenceid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: freak
--

ALTER TABLE ONLY public.unavailability
    ADD CONSTRAINT unavailablity_residenceid_fkey FOREIGN KEY (residenceid) REFERENCES public.residence(id) ON DELETE CASCADE;


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: freak
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
