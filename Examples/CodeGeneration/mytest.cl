class A {

  f() : Int {
    1
  };

};

class Main inherits IO {

  --g() : String {
  --  "HI"
  --};

  --h() : Int {
  --  (new Int)
  --};

  f() : Object {
    --if 1 = 2 then 7 else "hola" fi
    --if 1 = 2 then g() else h() fi
    --2
    "HOLA"
    --true
    --(new A)
  };

  g() : Object { 3 };

  h() : Object { true };

  j() : Object { (new A) };

  main(): IO {{
    --out_string("HI");

    --let x : Object <- f() in out_string(x.type_name());

    --let x : Object in x <- f();

    let x : Object <- f() in out_string(x.type_name());
    let x : Object <- j() in out_string(x.type_name());
    let x : Object <- g() in out_string(x.type_name());
    let x : Object <- h() in out_string(x.type_name());

    --case "HOLA" of
    --  o : Object => o;
    --  s : String => s;
    --  i : Int => i;
    --esac;

  }};
};