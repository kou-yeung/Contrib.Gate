//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class PetListSend {


	// params -> PetListSend
	static Parse(params : any): PetListSend {
		return JSON.parse(params.data[0]) as PetListSend;
	}
}

class PetListReceive {
    items: PetItem[]; // 所持ペット一覧
	
	// PetListReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function PetList(params, context, done) {

    // 受信データをパースする
    let s = PetListSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new PetListReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/