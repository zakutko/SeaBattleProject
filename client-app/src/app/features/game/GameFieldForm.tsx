import { ErrorMessage, Formik } from "formik";
import { observer } from "mobx-react";
import { Form, Label } from "semantic-ui-react";
import MyTextInput from "../../common/form/MyTextInput";
import { useStore } from "../../stores/store";
import "./game.css";

export default observer(function GameFieldForm(){
    const {shootStore} = useStore();
    const token = localStorage.getItem('token');

    const onSubmit = async (values, {setErrors}) => {
        shootStore.fire(values)
        .catch(error => setErrors({error: "There is no such cell on the field!"}));
    }

    return(
        <>
        <div>
            <Formik
                initialValues={{ x: 0, y: 0, token, error: null}}
                onSubmit = {onSubmit}
                >
                {({ handleSubmit, isSubmitting, errors}) => (
                <Form className="form" onSubmit={() => {handleSubmit()}}>
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

                    <Form.Button loading={isSubmitting} positive content='Shoot' type="submit"></Form.Button>
                </Form>
                )}
            </Formik>
        </div> 
        </>
        )
})