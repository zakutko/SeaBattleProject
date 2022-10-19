import { observer } from "mobx-react"
import { Form } from "semantic-ui-react"

export default observer(function FieldForm(){

    return (
        <>
        <div>
            <Form className="form">
                <Form.Group inline>
                    <label>Ship size:</label>
                    <Form.Radio
                        label='One cell'
                        value='one'
                    />
                    <Form.Radio
                        label='Two cells'
                        value='two'
                    />
                    <Form.Radio
                        label='Three cells'
                        value='three'
                    />
                    <Form.Radio
                        label='Four cells'
                        value='four'
                    />
                </Form.Group>
                <Form.Group inline>
                    <label>Ship direction:</label>
                    <Form.Radio
                        label='Horizontal (Right)'
                        value='horizontal'
                    />
                    <Form.Radio
                        label='Vertical (Down)'
                        value='vertival'
                    />
                </Form.Group>
                <Form.Group>
                    <Form.Input className="form-input" fluid label='Vertical:' placeholder='' />
                    <Form.Input className="form-input" fluid label='Horizontal:' placeholder=''/>
                </Form.Group>
                <Form.Button>Build a ship</Form.Button>
            </Form>
        </div> 
        </>
    )
})