class Main inherits IO {
  a : Int <- 2;
  c : Bool <- true;
  s : String <- "this is a";
  main() : IO { {
    a <- in_int();
    out_int(a);
    --a <- a * a;
    --out_int(29)
    --a.type_name();
    --s <- in_string();
    --s.concat(" string\n");
    --s.length();
    --out_int(4);
    --out_string("\n".concat(s.concat(" string\n")));
    --out_string(s.substr(5, 2).concat("\n"));
  } };
};
