CREATE TABLE "user" (
	user_id	uuid,
	first_name CHARACTER VARYING(256) NOT NULL,
	last_name VARCHAR(256) NOT NULL,
	created_by VARCHAR(256),
	created_on TIMESTAMP,
	updated_by CHARACTER VARYING(256),
	updated_on TIMESTAMP,
	CONSTRAINT pk_user PRIMARY KEY(user_id)
);

CREATE TABLE tag(
	"id" SERIAL NOT NULL,
	"Name" VARCHAR(256) NOT NULL,
	description VARCHAR(2000),
	CONSTRAINT pk_tag PRIMARY KEY("id")
);

CREATE TABLE blog(
	"id" SERIAL,
	title VARCHAR(1000) NOT NULL,
	"content" TEXT NOT NULL,
	created_by UUID NOT NULL,
	created_on TIMESTAMP NOT NULL,
	updated_by UUID NULL,
	updated_on TIMESTAMP NULL,
	CONSTRAINT pk_blog PRIMARY KEY("id"),
	CONSTRAINT fk_blog_user FOREIGN KEY(created_by) REFERENCES "user"(user_id)
);

CREATE TABLE post(
	"id" SERIAL NOT NULL,
	"Content" TEXT NOT NULL,
	created_by UUID NOT NULL,	
	created_on TIMESTAMP NOT NULL,
	updated_by UUID NULL,
	updated_on TIMESTAMP NULL,
	post_id INT NOT NULL,
	CONSTRAINT pk_post PRIMARY KEY("id"),
	CONSTRAINT fk_post_user FOREIGN KEY(created_by) REFERENCES "user"(user_id),
	CONSTRAINT fk_post_blog FOREIGN KEY(post_id) REFERENCES blog("id")
);
