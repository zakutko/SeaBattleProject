import { createContext, useContext } from "react";
import ShipStore from "./shipStore";
import UserStore from "./userStore";
import CommonStore from "./сommonStore";

interface Store {
    commonStore: CommonStore;
    userStore: UserStore;
    shipStore: ShipStore;
}

export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    shipStore: new ShipStore()
}

export const StoreContext = createContext(store);

export function useStore(){
    return useContext(StoreContext);    
}