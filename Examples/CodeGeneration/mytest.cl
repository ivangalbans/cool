class A {

  f() : Int {
    1
  };

};

class B inherits A{
  f() : Int {
    2
  };

  g() : Int {
    3
  };
};

class C inherits B{
  f() : Int {
    4
  };

  g() : Int {
    5
  };
};

class D inherits A{
  f() : Int {
    6
  };
};

class E inherits D{
};

class Main inherits IO {

  t(x:Object) : Object {{

    case x of
      o : Object => out_string( o.type_name() );
      a : A => out_int( a.f() );
      --b : B => out_string( "B\n" );
      --c : C => out_string( "C\n" );
      d : D => out_string( "D\n" );
      --e : E => out_string( "E\n" );
      s : String => out_string( s );
      i : Int => out_int(i);
    esac;

  }};

  main():Object {{
    t(new A);
    out_string("\n");
    t(new B);
    out_string("\n");
    t(new C);
    out_string("\n");
    t(new D);
    out_string("\n");
    t(new E);
    out_string("\n");
    t(7);
    out_string("\n");
    t(true);
    out_string("\n");
    t("Hola");
    out_string("\n");
  }};
};