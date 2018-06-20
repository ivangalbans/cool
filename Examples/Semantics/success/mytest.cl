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
  h(x:A) : Int {{
    x.f();
  }};

  t(x:A) : Object {{

    case x of
      o : Object => out_string( "Object\n" );
      a : A => out_string( "A\n" );
      b : B => out_string( "B\n" );
      c : C => out_string( "C\n" );
      d : D => out_string( "D\n" );
      e : E => out_string( "E\n" );
    esac;

  }};

  main():Object {{
    out_int((new A).f());
    out_int((new B).f());
    out_int(h(new A));
    out_int(h(new B));
    out_int(h(new C));
    out_int(h(new D));
    out_int(h(new E));
  }};
};