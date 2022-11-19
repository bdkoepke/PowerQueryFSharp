section Section1;

shared etfdb = MySQL.Database("mysql.alphaarchitect.com", "etfdb", [ReturnSingleDatabase=true]);
shared world = "World";
shared brandon = [Age=32, First ="Brandon"];
shared people = #table(type table [Age=Int64.Type, Name=text], {{32, "Brandon"}, {29, "Yan"}});