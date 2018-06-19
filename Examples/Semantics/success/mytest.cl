class Hello {

};

class Main inherits IO {
  --a:Int <- 5;
  --b:String <- "HI";
  --c:Bool <- true;
  --d:Int <- 9;
  --a : String;
  main():Object {{
    out_string((new Hello).type_name());

    --out_string("hola".substr(1,2));
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