import { observer } from "mobx-react"
import RowCellNumber from "./RowCellNumber"
import SecondPlayerFieldCellRow from "./SecondPlayerFieldCellRow"

export default observer(function SecondPlayerFieldCell(){
    return (
        <>
        <RowCellNumber />
        <SecondPlayerFieldCellRow Y={1} number={1}/>
        <SecondPlayerFieldCellRow Y={2} number={2}/>
        <SecondPlayerFieldCellRow Y={3} number={3}/>
        <SecondPlayerFieldCellRow Y={4} number={4}/>
        <SecondPlayerFieldCellRow Y={5} number={5}/>
        <SecondPlayerFieldCellRow Y={6} number={6}/>
        <SecondPlayerFieldCellRow Y={7} number={7}/>
        <SecondPlayerFieldCellRow Y={8} number={8}/>
        <SecondPlayerFieldCellRow Y={9} number={9}/>
        <SecondPlayerFieldCellRow Y={10} number={10}/>
        </>
    )
})