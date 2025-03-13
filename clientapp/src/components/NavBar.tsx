import { Link } from "react-router-dom";

const NavBar = () => {
  const list = [
    { "/": "Home" },
    { "/second": "Second" },
    { "/blablablajsnda": "Trasig" },
  ];

  return (
    <nav className="navbar">
      <ul>
        {list.map((x) => {
          const [key, value] = Object.entries(x)[0];
          return (
            <li>
              <Link to={key}>{value}</Link>
            </li>
          );
        })}
      </ul>
    </nav>
  );
};

export default NavBar;
