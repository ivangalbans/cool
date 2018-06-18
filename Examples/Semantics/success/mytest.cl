class Main inherits IO {
  x : Int <- 5;
  main():IO {
    let x : Int <- 6 in {
      out_int(x <- x + 1);
    }
  };
};
