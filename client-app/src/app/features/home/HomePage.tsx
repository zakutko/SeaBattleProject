import { observer } from "mobx-react";
import { Link } from "react-router-dom";
import { Button, Container, Header } from "semantic-ui-react";
import NavBar from "../../layout/NavBar";
import { useStore } from "../../stores/store";
import "../home/homePage.css";

export default observer(function HomePage(){
    const{userStore} = useStore();
    
    return (
            <Container textAlign="center" className="home">
                <Header as={'h1'} size='huge' inverted color='purple'>
                    SeaBattle Game
                </Header>
                {userStore.isLoggedIn ? (
                    <>
                        <NavBar/>
                        <Header as={'h2'} inverted content="Welcome to my sea battle game!" color="purple"/>
                    </>
                ) : (
                    <>
                    <Button as={Link} to='/login' size='medium' color="grey">
                        Login
                    </Button>
                    <Header as={'h4'}>If you don't have an account, please <Link to={'/register'}>register*</Link></Header>
                    </>
                )}
            </Container>
    )
})