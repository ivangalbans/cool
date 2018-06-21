
class Main inherits IO {

  f(x:Object) : IO {
    out_string(x.type_name().concat("\n"))
  };

  main(): IO {{
    f(2);
    f(5);
    f(true);
    f(7);
    f("hi");
    f(false);
    f(new Main);
    f(false);
    f(7);
    f("by");
  }};
};