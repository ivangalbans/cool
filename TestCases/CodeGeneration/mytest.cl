class Main inherits IO {

  g() : String {
    "HI"
  };

  h() : Int {
    (new Int)
  };

  f() : Object {
    --if 1 = 2 then 7 else "hola" fi
    --if 1 = 2 then g() else h() fi
    --2
    (new A);
  };

  main():Object {{
    --out_string("HI");

    --let x : Object <- f() in out_string(x.type_name());

    --let x : Object in x <- f();

    let x : Object <- f() in out_string(x.type_name());

    --case "HOLA" of
    --  o : Object => o;
    --  s : String => s;
    --  i : Int => i;
    --esac;

  }};
};