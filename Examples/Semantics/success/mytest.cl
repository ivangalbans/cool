class Main inherits IO {
  x : Int <- 5;
  main():IO {{
    let x : Int <- 1 in {
      out_int(x);
      out_string("\n");
      
      x <- 2;
      
      out_int(x);
      out_string("\n");

      out_int(x);
      out_string("\n");
    };
  }};
};
