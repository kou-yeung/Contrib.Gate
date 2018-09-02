//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class FinishPrologueSend {


	// params -> FinishPrologueSend
	static Parse(params : any): FinishPrologueSend {
		return JSON.parse(params.data[0]) as FinishPrologueSend;
	}
}

class FinishPrologueReceive {
    step: UserCreateStep;
	
	// FinishPrologueReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}