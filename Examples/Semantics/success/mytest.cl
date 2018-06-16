class Main inherits IO {
  --s : String <- "this is a";
  main() : Object { {
    out_int(4);
    --out_string("\n".concat(s.concat(" string\n")));
    --out_string(s.substr(5, 2).concat("\n"));
  } };
};
