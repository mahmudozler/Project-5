import time
from urllib.request import urlopen as uReq
from bs4 import BeautifulSoup as soup

input("Press ENTER to start...")

start_time = time.clock()

target_urls = ["https://www.coolblue.nl/producttype:laptops", "https://www.coolblue.nl/producttype:desktops", "https://www.coolblue.nl/producttype:monitoren",
               "https://www.coolblue.nl/producttype:muizen,gaming-muizen", "https://www.coolblue.nl/producttype:toetsenborden", "https://www.coolblue.nl/producttype:televisies",
               "https://www.coolblue.nl/producttype:hoofdtelefoons", "https://www.coolblue.nl/producttype:oordopjes", "https://www.coolblue.nl/producttype:bluetooth-speakers",
               "https://www.coolblue.nl/producttype:mobiele-telefoons", "https://www.coolblue.nl/producttype:tablets", "https://www.coolblue.nl/producttype:consoles",
               "https://www.coolblue.nl/producttype:speakers", "https://www.coolblue.nl/producttype:spiegelreflexcameras,compactcameras,systeemcameras", "https://www.coolblue.nl/producttype:home-cinema-sets",
               "https://www.coolblue.nl/producttype:action-cameras"]

product_count = 0

filename = "products.csv"
f = open(filename, "w")
f.write("product_type, product_name, product_price\n")


def pageSoup(target):
    uClient = uReq(target)
    page_html = uClient.read()
    uClient.close()

    return soup(page_html, "html5lib")


for target in target_urls:
    page_soup_main = pageSoup(target)
    paginations = page_soup_main.find(
        "ul", {"class": "pagination js-pagination"}).findAll("li")
    page_count = int(paginations[len(paginations) - 2].text.strip())

    for i in range(page_count):
        page_soup = pageSoup(target + "?pagina={0}".format(i + 1))
        cards = page_soup.findAll("div", {"class": "product-grid__item card"})

        print("    " + target + "?pagina={0}".format(i + 1))

        for card in cards:
            product_count = product_count + 1
            product_type = card.find(
                "div", {"class": "product__display"}).a["data-producttypename"].replace("\"", "''")
            product_name = card.find(
                "div", {"class": "product__details"}).div.a.text.strip().replace("\"", "''")
            product_price = float(card.find(
                "strong", {"class": "product__sales-price"}).text.strip().replace(".", "").replace(",", ".").replace("-", "00"))

            #print("product_type: " + product_type)
            print("product_name: " + product_name)
            #print("product_price: " + product_price)

            f.write("\"{0}\"".format(product_type) + "," + "\"{0}\"".format(
                product_name) + "," + "€ {0:.2f}".format(product_price) + "\n")

f.close()

print("found {0} products".format(product_count))
print("{0}s".format(time.clock() - start_time))
