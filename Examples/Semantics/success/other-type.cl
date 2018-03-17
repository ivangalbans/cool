Class Complex inherits IO {
    a : Int;
    b : Int;

    toString() : String {
        {
            out_string(a);
            out_string(" + ");
            out_string(b);
            out_string("i");
        }
    };

    sum(other : Complex) : Complex {
        out_string("sumando")
    };

};

Class Main inherits IO {
    number1 : Complex;
    number2 : Complex;
};