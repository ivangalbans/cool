class Main inherits IO {


  main():Object {{
    --out_int(2);
    --out_string("hola");

    --let x:Int <- in_int(), y:String <- in_string() in {
    --  out_int(x*x);
    --  out_string(y.concat("HI"));
    --};

    out_string("holamundo".substr(0,5));
    out_string("\n");
    out_string("holamundo".substr(1,5));
    out_string("\n");
    out_string("holamundo".substr(2,5));
    out_string("\n");
    out_string("holamundo".substr(4,5));
    out_string("\n");
    out_string("holamundo".substr(5,5));
    out_string("\n");

    --t(2.copy());
    --t((new A).copy());
    --let x:Int <- 2, y:Int <- x * 8, z:Int <- y/3 in out_int(z);
  }};
};