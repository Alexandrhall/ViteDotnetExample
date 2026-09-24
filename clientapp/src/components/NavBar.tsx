import { Link } from "react-router-dom";

const NavBar = () => {
  const list = [
    { "/": "Home" },
    { "/second": "Second" },
    { "/blablablajsnda": "Broken" },
  ];

  return (
    <nav className="navbar">
      <ul>
        {list.map((x) => {
          const [key, value] = Object.entries(x)[0];
          return (
            <li key={key}>
              <Link to={key}>{value}</Link>
            </li>
          );
        })}
      </ul>
    </nav>
  );
};

export default NavBar;
