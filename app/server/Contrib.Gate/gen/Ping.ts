//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class PingSend {
    message: string;

	// params -> PingSend
	static Parse(params : any): PingSend {
		return JSON.parse(params.data[0]) as PingSend;
	}
}

class PingReceive {
    message: string;
    timestamp: string;
	
	// PingReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}