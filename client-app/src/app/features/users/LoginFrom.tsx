import { ErrorMessage, Formik } from "formik";
import { observer } from "mobx-react";
import { useNavigate } from "react-router";
import { Button, Form, Label } from "semantic-ui-react";
import MyTextInput from "../../common/form/MyTextInput";
import { useStore } from "../../stores/store";
import "../home/homePage.css";

export default observer(function LoginForm() {
    const {userStore} = useStore();
    const navigate = useNavigate();

    const onSubmit = async (values, {setErrors}) => {
        userStore.login(values)
        .catch(error => setErrors({error: "Invalid email or password!"}));
    }

    return (
        <div className="login-register-form">
            <Formik 
            initialValues={{email: '', password: '', error: null}}
            onSubmit = {onSubmit}
        >
            {({handleSubmit, isSubmitting, errors}) => (
                <Form className="ui form" onSubmit={() => {handleSubmit(); navigate("/")}} autoComplete='off'>
                    <MyTextInput name='email' placeholder='Email'/>
                    <MyTextInput name='password' placeholder='Password' type="password"/>

                    <ErrorMessage
                        name="error" render={() => 
                        <Label 
                            style={{marginBottom: 10}} basic color='red' content={errors.error} 
                        />}  
                    />

                    <Button loading={isSubmitting} positive content='Login' type="submit" fluid />
                </Form>
            )}
        </Formik>
        </div>
    )
})