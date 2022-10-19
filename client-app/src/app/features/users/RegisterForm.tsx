import { ErrorMessage, Formik } from "formik";
import { observer } from "mobx-react";
import { Button, Form, Label } from "semantic-ui-react";
import MyTextInput from "../../common/form/MyTextInput";
import { useStore } from "../../stores/store";
import * as Yup from 'yup';


export default observer(function RegisterForm() {
    const {userStore} = useStore();

    return (
        
        <Formik 
            initialValues={{displayName: '', username: '', email: '', password: '', error: null}}
            onSubmit = {(values, {setErrors}) => userStore.register(values).catch(error => setErrors({error: "User with such email already exists!"}))}
            validationSchema={Yup.object({
                displayName: Yup.string().required(),
                username: Yup.string().required(),
                email: Yup.string().required(),
                password: Yup.string().required()
            })}
        >
            {({handleSubmit, isSubmitting, errors, isValid, dirty}) => (
                <Form className="ui form" onSubmit={handleSubmit} autoComplete='off'>
                    <MyTextInput name='displayName' placeholder='Display Name'/>
                    <MyTextInput name='username' placeholder='Username'/>
                    <MyTextInput name='email' placeholder='Email'/>
                    <MyTextInput name='password' placeholder='Password' type="password"/>

                    <ErrorMessage
                        name="error" render={() => 
                        <Label 
                            style={{marginBottom: 10}} basic color='red' content={errors.error} 
                        />}  
                    />

                    <Button disabled={!isValid || !dirty || isSubmitting} 
                        loading={isSubmitting} positive content='Register' type="submit" fluid/>
                </Form>
            )}
        </Formik>
    )
})