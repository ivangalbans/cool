class Main inherits IO {
  a : Int <- 2;
  s : String <- "this is a";
  main() : Int { {
    --a <- a * a;
    --out_int(29)
    a.copy();
    a.abort();
    s.abort();
    17;
    --s <- in_string();
    --s.concat(" string\n");
    --s.length();
    --out_int(4);
    --out_string("\n".concat(s.concat(" string\n")));
    --out_string(s.substr(5, 2).concat("\n"));
  } };
};
