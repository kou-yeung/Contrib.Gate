//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class LoginSend {


	// params -> LoginSend
	static Parse(params : any): LoginSend {
		return JSON.parse(params.data[0]) as LoginSend;
	}
}

class LoginReceive {
    state: number;
    flags: boolean[];
	
	// LoginReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}