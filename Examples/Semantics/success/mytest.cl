class Main inherits IO {

  f() : Object{
    if 1 = 2 then 7 else "hola" fi
  };

  main():Object {{

    let x : Object <- f() in out_string(x.type_name());

    case "HOLA" of
      o : Object => o;
      s : String => s;
      i : Int => i;
    esac;

  }};
};