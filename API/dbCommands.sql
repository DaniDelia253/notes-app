CREATE TABLE notes (
note_id INTEGER AUTO_INCREMENT,
user_id INTEGER,
title TEXT,
content TEXT,
PRIMARY KEY (note_id),
FOREIGN KEY (user_id) REFERENCES users(Id)
);

INSERT INTO users (email, username, passwordSalt, passwordHash) VALUES ("alicia@email.com", "alicia", "aliciaPasswordSalt", "aliciaPasswordHash");

INSERT INTO notes (user_id, title, content)VALUES (1, "My first note", "this is the content of my first note");
INSERT INTO notes (user_id, title, content)VALUES (1, "This is my second note", "even more content!!!!");
INSERT INTO notes (user_id, title, content)VALUES (2, "user2note", "this is what user 2 wrote!!!");
INSERT INTO notes (user_id, title, content)VALUES (1, "My third note", "third is the one with the ___________");


SELECT email, username FROM users WHERE email = \"${}\" OR username = \"${}\";
SELECT email, username FROM users WHERE email = "" OR username = "sten";