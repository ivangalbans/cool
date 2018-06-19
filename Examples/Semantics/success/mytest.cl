class Main inherits IO {
  --a : String;
  main():Object {{
    out_string("hola".substr(1,2));
    --out_string("hola".concat(" ").concat("mundo").concat(".").concat(" ").concat("adios").concat("."));
    --out_int("hola".length());
    --out_string(a);
    --out_int(a.length());
    --a <- "HELLO\n";
    --out_int(a.length());
    --out_string(a);
    --a <- in_string();
    --out_string(a);
  }};
};