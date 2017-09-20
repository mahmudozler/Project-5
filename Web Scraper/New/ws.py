import re
import time
from urllib.request import urlopen as uReq
from bs4 import BeautifulSoup as soup

input("Press ENTER to start...")

start_time = time.clock()

website = "https://www.coolblue.nl"
target_urls = ["https://www.coolblue.nl/producttype:laptops"]

# target_urls = ["https://www.coolblue.nl/producttype:laptops", "https://www.coolblue.nl/producttype:desktops", "https://www.coolblue.nl/producttype:monitoren",
#                "https://www.coolblue.nl/producttype:muizen,gaming-muizen", "https://www.coolblue.nl/producttype:toetsenborden", "https://www.coolblue.nl/producttype:televisies",
#                "https://www.coolblue.nl/producttype:hoofdtelefoons", "https://www.coolblue.nl/producttype:oordopjes", "https://www.coolblue.nl/producttype:bluetooth-speakers",
#                "https://www.coolblue.nl/producttype:mobiele-telefoons", "https://www.coolblue.nl/producttype:tablets", "https://www.coolblue.nl/producttype:consoles",
#                "https://www.coolblue.nl/producttype:speakers", "https://www.coolblue.nl/producttype:spiegelreflexcameras,compactcameras,systeemcameras", "https://www.coolblue.nl/producttype:home-cinema-sets",
#                "https://www.coolblue.nl/producttype:action-cameras"]

product_count = 0

filename = "product_laptops.csv"
f = open(filename, "w")
f.write("product_type,product_name,product_price,product_brand,product_model,product_screen,product_screenres,product_cpu,product_graphicscard,product_graphicsmemory,product_memory,product_storage,product_dimensions,product_weight\n")


def pageSoup(target):
    uClient = uReq(target)
    page_html = uClient.read()
    uClient.close()

    return soup(page_html, "html5lib")


def formatCSV(text):
    return "\"{0}\",".format(text)


def getSpec(spec):
    try:
        return card_detail.find(text=re.compile(spec)).parent.parent.dd.text.strip().replace("\"", "''")
    except:
        try:
            return card_detail.find(text=re.compile(spec)).parent.parent.parent.parent.dd.text.strip().replace("\"", "''")
        except:
            return "None"


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
            card_detail = pageSoup(
                website + card.find("a", {"class": "product__title js-product-title"})["href"])

            product_count = product_count + 1
            product_type = card.find(
                "div", {"class": "product__display"}).a["data-producttypename"].replace("\"", "''")
            product_name = card.find(
                "div", {"class": "product__details"}).div.a.text.strip().replace("\"", "''")
            product_price = float(card.find(
                "strong", {"class": "product__sales-price"}).text.strip().replace(".", "").replace(",", ".").replace("-", "00"))
            product_brand = getSpec("Merk")
            product_model = getSpec("Fabrikantcode")
            product_screen = getSpec("Schermdiagonaal")
            product_screenres = getSpec("Resolutie")
            prodict_cpu = getSpec("Processor") + " " + \
                getSpec("Processornummer")
            product_graphicscard = getSpec("Videokaart")
            product_graphicsmemory = getSpec("Videogeheugen")
            product_memory = getSpec("RAM-geheugen")
            product_storage = getSpec("Opslagcapaciteit")
            product_dimensions = "{0} x {1} x {2}".format(getSpec("Breedte"),
                                                          getSpec("Diepte"),
                                                          getSpec("Hoogte"))
            product_weight = getSpec("Gewicht")

            #print("product_type: " + product_type)
            print("product_name: " + product_name)
            #print("product_price: " + product_price)

            f.write(formatCSV(product_type) + formatCSV(product_name) + "€ {0:.2f},".format(product_price) +
                    formatCSV(product_brand) + formatCSV(product_model) + formatCSV(product_screen) +
                    formatCSV(product_screenres) + formatCSV(prodict_cpu) + formatCSV(product_graphicscard) +
                    formatCSV(product_graphicsmemory) + formatCSV(product_memory) + formatCSV(product_storage) +
                    formatCSV(product_dimensions) + formatCSV(product_weight) + "\n")

f.close()

print("found {0} products".format(product_count))
print("{0}s".format(time.clock() - start_time))
