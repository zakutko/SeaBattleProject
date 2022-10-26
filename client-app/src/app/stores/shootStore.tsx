import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { ShootFormValues } from "../models/shoot";

export default class ShootStore {
    constructor(){
        makeAutoObservable(this)
    }

    fire = async (creds: ShootFormValues) => {
        try {
            const shoot = await agent.Games.fire(creds);
            runInAction(() => shoot);
            console.log(shoot);
        }
        catch(error){
            throw error;
        }
    }
}