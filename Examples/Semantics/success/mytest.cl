class Main inherits IO {
  main():Object {{
    --if "hola" = "hola" then out_string("HI\n") else out_string("BYTE\n") fi;
    --if "holas" = "hola" then out_string("HI\n") else out_string("BYTE\n") fi;
    --if "hola" = "holas" then out_string("HI\n") else out_string("BYTE\n") fi;
    --if "adios" = "ok" then out_string("HI\n") else out_string("BYTE\n") fi;
    --if "okok" = "ok" then out_string("HI\n") else out_string("BYTE\n") fi;
    --if "ok" = "ok" then out_string("HI\n") else out_string("BYTE\n") fi;

    let a:String <- in_string(), b:String <- in_string() in
    {
      --out_string(a.concat("\n"));
      --out_string(b.concat("\n"));
      if a = b then out_string("equals\n") else out_string("diff\n") fi;
    };

  }};
};