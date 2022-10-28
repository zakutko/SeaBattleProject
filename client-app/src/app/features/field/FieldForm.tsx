import { ErrorMessage, Formik } from "formik";
import { observer } from "mobx-react"
import { Form, Label } from "semantic-ui-react"
import MyTextInput from "../../common/form/MyTextInput";
import { useStore } from "../../stores/store";
import { FormControl, FormControlLabel, Radio, RadioGroup } from "@mui/material";
import { useNavigate } from "react-router";

export default observer(function FieldForm(){
    const {shipStore} = useStore();

    enum SizeOptions {
        "One" = 1,
        "Two" = 2,
        "Three" = 3,
        "Four" = 4
    }

    enum Options {
        "Horizontal" = 1,
        "Vertical" = 2
    }

    const name = 'shipDirection';
    const nameSize = 'shipSize';

    const token = localStorage.getItem('token');
    const navigate = useNavigate();

    return (
        <>
        <div>
            <Formik
                initialValues={{shipSize: SizeOptions.One, shipDirection: Options.Horizontal, x: 0, y: 0, token, error: null}}
                onSubmit = {(values, {setErrors}) => {
                    shipStore.createShipOnField(values).catch(error => setErrors({error: "An error occured while adding a ship!"}));
                }}
                >
                {({ values, setFieldValue, handleSubmit, isSubmitting, errors}) => (
                <Form className="form" onSubmit={() => {handleSubmit(); navigate(0)}}>
                    <h2>Ship size:</h2>
                    <FormControl component="fieldset">
                        <RadioGroup name={nameSize} value={values.shipSize} onChange={(event) => {
                            setFieldValue(nameSize, event.currentTarget.value)
                        }}>
                            <FormControlLabel value={SizeOptions.One} control={<Radio />} label="One" />
                            <FormControlLabel value={SizeOptions.Two} control={<Radio />} label="Two" />
                            <FormControlLabel value={SizeOptions.Three} control={<Radio />} label="Three" />
                            <FormControlLabel value={SizeOptions.Four} control={<Radio />} label="Four" />
                        </RadioGroup>
                    </FormControl>
                    <h2>Ship direction:</h2>
                    <FormControl component="fieldset">
                        <RadioGroup name={name} value={values.shipDirection} onChange={(event) => {
                            setFieldValue(name, event.currentTarget.value)
                        }}>
                            <FormControlLabel value={Options.Horizontal} control={<Radio />} label="Horizontal" />
                            <FormControlLabel value={Options.Vertical} control={<Radio />} label="Vertical" />
                        </RadioGroup>
                    </FormControl>
                    <h2>Ship Position:</h2>
                    <Form.Group>
                        <MyTextInput name='x' placeholder='X' label="X:"/>
                        <MyTextInput name='y' placeholder='Y' label="Y:"/>
                    </Form.Group>

                    <ErrorMessage
                        name="error" render={() => 
                        <Label 
                            style={{marginBottom: 10}} basic color='red' content={errors.error} 
                        />}  
                    />

                    <Form.Button loading={isSubmitting} positive content='Build a ship' type="submit"></Form.Button>
                </Form>
                )}
            </Formik>
        </div> 
        </>
    )
})