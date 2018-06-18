class Main inherits IO {
  a : String;
  main():Object {{
    out_string(a);
    a <- "HELLO\n";
    out_string(a);
    a <- in_string();
    out_string(a);
  }};
};