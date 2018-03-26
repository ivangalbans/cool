
class Main0 inherits IO {
    a : Int;
    main() : String {
        out_string(a.type_name()) --print 'a'
    };
};

-- error in runtime

class Main1 inherits IO {
    a : Main0;
    main() : String {
        out_string(a.type_name())
    };
};

class Main {
	main() : Int {
		1
	};
};