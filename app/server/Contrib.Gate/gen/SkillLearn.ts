//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class SkillLearnSend {
    uniqid: string; // ペットUniqid
    skill: number; // スキルID

	// params -> SkillLearnSend
	static Parse(params : any): SkillLearnSend {
		return JSON.parse(params.data[0]) as SkillLearnSend;
	}
}

class SkillLearnReceive {
    items: InventoryItem[]; // 更新したアイテム一覧
    pet: PetItem; // 更新したペット情報
	
	// SkillLearnReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function SkillLearn(params, context, done) {

    // 受信データをパースする
    let s = SkillLearnSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new SkillLearnReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/