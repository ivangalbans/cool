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
      s : String => out_string( s );
      i : Int => out_int(i);
    esac;

  }};

  main():Object {{
    out_int(2);
    out_string("hola");

    let x:Int <- in_int(), y:String <- in_string() in {
      out_int(x*x);
      out_string(y.concat("HI"));
    };

    --t(2.copy());
    --t((new A).copy());
    --let x:Int <- 2, y:Int <- x * 8, z:Int <- y/3 in out_int(z);
  }};
};